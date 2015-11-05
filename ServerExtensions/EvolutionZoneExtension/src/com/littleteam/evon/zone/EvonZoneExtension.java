/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.zone;

import com.littleteam.evon.utils.*;
import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.extensions.SFSExtension;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.List;

/**
 * Зона по умолчанию
 *
 * @author PanCrucian
 */
public class EvonZoneExtension extends SFSExtension {

    @Override
    public void init() {
        //события сервера
        addEventHandler(SFSEventType.USER_LOGIN, LoginEventHandler.class);
        addEventHandler(SFSEventType.USER_JOIN_ZONE, JoinEventHadler.class);
        addEventHandler(SFSEventType.USER_DISCONNECT, DisconnectHadler.class);
        //события клиента
        addRequestHandler(String.valueOf(Requests.Types.FacebookUserData), FacebookUserDataHadler.class);
        addRequestHandler(String.valueOf(Requests.Types.Ping), PingHandler.class);
        addRequestHandler(String.valueOf(Requests.Types.Pong), PongHandler.class);
        //хук на выключение сервера
        Runtime.getRuntime().addShutdownHook(new Thread() {
            @Override
            public void run() {
                OnShutdown();
            }
        });
        Log.Info(this, "Evon Zone Extension Initalized");
    }

    /**
     * При выключении сервера
     */
    private void OnShutdown() {
        List<User> users = (List<User>) getParentZone().getUserList();
        users.stream().forEach((user) -> {
            RecordSessionTime(user);
        });
    }

    /**
     * Юзер отключился
     *
     * @param user
     */
    public void OnUserDisconnect(User user) {
        RecordSessionTime(user);
    }

    /**
     * Запишем игровую сессию в БД
     *
     * @param user
     */
    private void RecordSessionTime(User user) {
        Connection conn = null;
        PreparedStatement stmt = null;

        String userName = user.getName();
        int userLoginTime = user.getVariable(UserVariables.TIME_LOGIN).getIntValue();
        int currentTime = Time.GetUnixStampSeconds();

        try {
            conn = getParentZone().getDBManager().getConnection();
            stmt = conn.prepareStatement(
                    "INSERT INTO `" + DBTables.Sessions + "` "
                    + "SET "
                    + "user_id = ?,"
                    + "session_start = ?,"
                    + "session_length = ?");
            stmt.setInt(1, Integer.parseInt(userName));
            stmt.setInt(2, userLoginTime);
            stmt.setInt(3, currentTime - userLoginTime);
            stmt.executeUpdate();
        } catch (SQLException ex) {
            Log.Error(this, ex.getMessage());
        } finally {
            try {
                if (stmt != null) {
                    stmt.close();
                }
            } catch (SQLException ex) {
                Log.Error(this, ex.getMessage());
            }
            try {
                if (conn != null) {
                    conn.close();
                }
            } catch (SQLException ex) {
                Log.Error(this, ex.getMessage());
            }
        }
    }
}
