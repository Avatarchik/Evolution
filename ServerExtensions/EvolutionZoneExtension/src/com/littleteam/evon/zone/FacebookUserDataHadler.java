/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.zone;

import com.littleteam.evon.serialization.*;
import com.littleteam.evon.utils.DBTables;
import com.littleteam.evon.utils.Log;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.util.ClientDisconnectionReason;;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;

/**
 *
 * @author PanCrucian
 */
public class FacebookUserDataHadler extends BaseClientRequestHandler {

    Connection conn;
    PreparedStatement stmt;

    @Override
    public void handleClientRequest(User user, ISFSObject params) {
        FacebookUserInfoMy facebookUserInfo = (FacebookUserInfoMy) params.getClass("FacebookUserInfoMy");
        if (facebookUserInfo.Id.length() == 0) {
            Log.Info(getParentExtension(), "facebookId lenght <= zero");
            return;
        }

        ISFSArray resultArr = new SFSArray();
        ISFSObject userObject = null;
        ISFSObject facebookObject = null;
        boolean facebookIdDifferent = false;
        boolean reloginRequired = false;

        do {
            try {
                //ищем по фейсбук ID
                conn = getParentExtension().getParentZone().getDBManager().getConnection();
                stmt = conn.prepareStatement("SELECT * FROM " + DBTables.UsersFacebook + " WHERE facebook_id = ?");
                stmt.setString(1, facebookUserInfo.Id);
                resultArr = SFSArray.newFromResultSet(stmt.executeQuery());

                //в БД больше двух аккаунтов фейсбук, это Баг
                if (resultArr.size() >= 2) {
                    break;
                }

                //если записи о аккаунте нет, то создадим
                if (resultArr.size() == 0) {
                    stmt = conn.prepareStatement(
                            "INSERT INTO `" + DBTables.UsersFacebook + "` "
                            + "SET "
                            + "facebook_id = ?,"
                            + "email = ?,"
                            + "first_name = ?,"
                            + "last_name = ?,"
                            + "gender = ?,"
                            + "locale = ?");
                    stmt.setString(1, facebookUserInfo.Id);
                    stmt.setString(2, facebookUserInfo.Email);
                    stmt.setString(3, facebookUserInfo.FirstName);
                    stmt.setString(4, facebookUserInfo.LastName);
                    stmt.setInt(5, facebookUserInfo.Gender);
                    stmt.setString(6, facebookUserInfo.Locale);
                    stmt.executeUpdate();
                } else {
                    //иначе обновим запись
                    stmt = conn.prepareStatement(
                            "UPDATE `" + DBTables.UsersFacebook + "` "
                            + "SET "
                            + "email = ?,"
                            + "first_name = ?,"
                            + "last_name = ?,"
                            + "gender = ?,"
                            + "locale = ? "
                            + "WHERE facebook_id = ?");
                    stmt.setString(1, facebookUserInfo.Email);
                    stmt.setString(2, facebookUserInfo.FirstName);
                    stmt.setString(3, facebookUserInfo.LastName);
                    stmt.setInt(4, facebookUserInfo.Gender);
                    stmt.setString(5, facebookUserInfo.Locale);
                    stmt.setString(6, facebookUserInfo.Id);
                    stmt.executeUpdate();
                }
                stmt = conn.prepareStatement("SELECT * FROM " + DBTables.UsersFacebook + " WHERE facebook_id = ?");
                stmt.setString(1, facebookUserInfo.Id);
                resultArr = SFSArray.newFromResultSet(stmt.executeQuery());
                facebookObject = resultArr.getSFSObject(0);

                //получим информацию из БД о юзере
                stmt = conn.prepareStatement("SELECT * FROM " + DBTables.Users + " WHERE id = ?");
                stmt.setInt(1, Integer.parseInt(user.getName()));
                resultArr = SFSArray.newFromResultSet(stmt.executeQuery());

                //если не нашли юзера, это Баг
                if (resultArr.size() == 0) {
                    break;
                }
                userObject = resultArr.getSFSObject(0);

                //аккаунты с одинаковым ID
                if (userObject.getUtfString("facebook_id").equals(facebookUserInfo.Id)) {
                    break;
                }

                //если до этого не было привязано ни одного FB аккаунта
                if (userObject.getUtfString("facebook_id").equals("")) {
                    //привяжем текущий и выйдем
                    stmt = conn.prepareStatement(
                            "UPDATE `" + DBTables.Users + "` "
                            + "SET "
                            + "facebook_id = ? "
                            + "WHERE id = ?");
                    stmt.setString(1, facebookUserInfo.Id);
                    stmt.setInt(2, Integer.parseInt(user.getName()));
                    stmt.executeUpdate();
                    reloginRequired = true;
                    break;
                }

                //аккаунты были привязаны и они не одинаковые
                facebookIdDifferent = true;

            } catch (SQLException ex) {
                Log.Error(getParentExtension(), ex.getMessage());
            } finally {
                try {
                    if (stmt != null) {
                        stmt.close();
                    }
                } catch (SQLException ex) {
                    Log.Error(getParentExtension(), ex.getMessage());
                }
                try {
                    if (conn != null) {
                        conn.close();
                    }
                } catch (SQLException ex) {
                    Log.Error(getParentExtension(), ex.getMessage());
                }
            }
        } while (false);

        if (facebookObject == null) {
            Log.Error(
                    getParentExtension(),
                    "В БД больше двух аккаунтов фейсбук с одинаковым ID: " + facebookUserInfo.Id + ". Это Баг " + FacebookUserDataHadler.class.getName()
            );
            user.disconnect(ClientDisconnectionReason.KICK);
            return;
        }

        if (userObject == null) {
            Log.Error(getParentExtension(), "Юзер не найден в БД, это баг " + FacebookUserDataHadler.class.getName());
            user.disconnect(ClientDisconnectionReason.KICK);
            return;
        }

        if (facebookIdDifferent)
            reloginRequired = true;

        if (reloginRequired) {
            Log.Info(getParentExtension(), "Установлен новый фейсбук аккаунт для пользователя: " + user.getName());
            user.disconnect(ClientDisconnectionReason.KICK);
        }
    }
}
