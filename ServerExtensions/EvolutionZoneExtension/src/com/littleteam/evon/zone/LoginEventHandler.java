/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.zone;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import com.littleteam.evon.utils.*;
import com.smartfoxserver.v2.core.SFSConstants;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.exceptions.*;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 *
 * @author PanCrucian
 */
public class LoginEventHandler extends BaseServerEventHandler {

    private class LoginData {
        String deviceId = "";
        String version = "";
        String platform = "";
        String facebookId = "";
    }
    LoginData loginData;
    Connection conn;
    PreparedStatement stmt;

    @Override
    public void handleServerEvent(ISFSEvent event) throws SFSException {
        ISFSObject customData = (ISFSObject) event.getParameter(SFSEventParam.LOGIN_IN_DATA);
        loginData = new LoginData();

        loginData.deviceId = (String) event.getParameter(SFSEventParam.LOGIN_NAME);
        loginData.version = customData.getUtfString("version");
        loginData.platform = customData.getUtfString("platform");
        loginData.facebookId = customData.getUtfString("facebookId");

        //проверим длину имени
        if (loginData.deviceId.length() != 32) {
            SFSErrorData errorData = new SFSErrorData(SFSErrorCodeExtended.USERNAME_LENGHT);
            errorData.addParameter(loginData.deviceId);
            throw new SFSLoginException("Incorrect username lenght. Username: {0}", errorData);
        }

        //проверим версию клиента
        if (!loginData.version.equals(Version.Server)) {
            SFSErrorData errorData = new SFSErrorData(SFSErrorCodeExtended.VERSION_NOT_VALID);
            errorData.addParameter(loginData.version);
            throw new SFSLoginException("Client version: {0} - invalid", errorData);
        }

        ISFSArray resultArr = new SFSArray();

        //пытаемся читать информацию в Базе Данных
        Boolean dbError = false;
        try {
            //ищем по фейсбуку
            conn = getParentExtension().getParentZone().getDBManager().getConnection();
            if (loginData.facebookId.length() > 0) {
                stmt = conn.prepareStatement("SELECT * FROM " + DBTables.Users + " WHERE facebook_id = ?");
                stmt.setString(1, loginData.facebookId);
                resultArr = SFSArray.newFromResultSet(stmt.executeQuery());
            }
            //если не нашли, ищем по device ID
            if (resultArr.size() == 0) {
                stmt = conn.prepareStatement("SELECT * FROM " + DBTables.Users + " WHERE device_id = ?");
                stmt.setString(1, loginData.deviceId);
                resultArr = SFSArray.newFromResultSet(stmt.executeQuery());
            }
            //если не нашли, то создаем запись о новом игроке
            if (resultArr.size() == 0) {
                stmt = conn.prepareStatement(
                        "INSERT INTO `" + DBTables.Users + "` "
                        + "SET "
                        + "device_id = ?");
                stmt.setString(1, loginData.deviceId);
                stmt.executeUpdate();

                stmt = conn.prepareStatement("SELECT * FROM " + DBTables.Users + " WHERE device_id = ?");
                stmt.setString(1, loginData.deviceId);
                resultArr = SFSArray.newFromResultSet(stmt.executeQuery());
            }
        } catch (SQLException ex) {
            Log.Error(getParentExtension(), ex.getMessage());
            dbError = true;
        } finally {
            try {
                if (stmt != null) {
                    stmt.close();
                }
            } catch (SQLException ex) {
                Log.Error(getParentExtension(), ex.getMessage());
                dbError = true;
            }
            try {
                if (conn != null) {
                    conn.close();
                }
            } catch (SQLException ex) {
                Log.Error(getParentExtension(), ex.getMessage());
                dbError = true;
            }
        }
        
        //были ошибки при запросе к БД, нельзя продолжать
        if (dbError) {
            throw new SFSLoginException("Database problem", new SFSErrorData(SFSErrorCodeExtended.DATABASE_ERROR));
        }
        
        ISFSObject userObject = resultArr.getSFSObject(0);
        String name = (String) event.getParameter(SFSEventParam.LOGIN_NAME);
        ISFSObject outData = (ISFSObject) event.getParameter(SFSEventParam.LOGIN_OUT_DATA);
        outData.putUtfString(SFSConstants.NEW_LOGIN_NAME, String.valueOf(userObject.getInt("id")));
    }
}
