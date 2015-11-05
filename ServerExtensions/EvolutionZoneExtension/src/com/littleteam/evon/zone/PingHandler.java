/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.zone;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.littleteam.evon.utils.*;
import com.smartfoxserver.v2.entities.data.SFSObject;

/**
 * Слушатель для работы с пингом
 *
 * @author PanCrucian
 */
public class PingHandler extends BaseClientRequestHandler {

    @Override
    public void handleClientRequest(User user, ISFSObject params) {
        long serverTime = Time.GetUnixStampMillisecond();        

        ISFSObject response = new SFSObject();
        response.putLong("time", serverTime);
        send(String.valueOf(Responses.Types.Pong), response, user);
    }

}
