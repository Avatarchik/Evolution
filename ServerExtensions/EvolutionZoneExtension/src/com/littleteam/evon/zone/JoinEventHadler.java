/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.zone;

import com.littleteam.evon.utils.*;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import java.sql.Connection;
import java.sql.PreparedStatement;

/**
 *
 * @author PanCrucian
 */
public class JoinEventHadler extends BaseServerEventHandler {

    Connection conn;
    PreparedStatement stmt;

    @Override
    public void handleServerEvent(ISFSEvent event) throws SFSException {
        User user = (User) event.getParameter(SFSEventParam.USER);
        user.setVariable(new SFSUserVariable(UserVariables.TIME_LOGIN, Time.GetUnixStampSeconds()));
        user.setVariable(new SFSUserVariable(UserVariables.PING, 0));
    }

}
