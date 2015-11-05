/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.zone;

import com.littleteam.evon.utils.Log;
import com.littleteam.evon.utils.Responses;
import com.littleteam.evon.utils.Time;
import com.littleteam.evon.utils.UserVariables;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.exceptions.SFSVariableException;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

/**
 *
 * @author PanCrucian
 */
public class PongHandler extends BaseClientRequestHandler{

    @Override
    public void handleClientRequest(User user, ISFSObject params) {
        long serverTime = Time.GetUnixStampMillisecond();
        long clientTime = params.getLong("time");
        
        long ping = Math.abs(serverTime - clientTime);
        try {
            user.setVariable(new SFSUserVariable(UserVariables.PING, ping));
        } catch (SFSVariableException ex) {
            Log.Warning(getParentExtension(), ex.getMessage());
        }

        ISFSObject response = new SFSObject();
        response.putLong("ping", ping);
        send(String.valueOf(Responses.Types.Ping), response, user);
    }
   
}
