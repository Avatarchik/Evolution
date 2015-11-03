/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.zone;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

/**
 *
 * @author PanCrucian
 */
public class LeaveUser extends BaseServerEventHandler {

    @Override
    public void handleServerEvent(ISFSEvent evt) throws SFSException {
            User user = (User) evt.getParameter(SFSEventParam.USER);
            /*if(user == null)
                    Players.GarbageCollector();*/
            EvonZoneExtension extension = (EvonZoneExtension) getParentExtension();
            extension.OnUserLeave(user);
    }
}
