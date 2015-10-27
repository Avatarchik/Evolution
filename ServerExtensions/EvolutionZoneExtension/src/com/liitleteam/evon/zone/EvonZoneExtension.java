/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.liitleteam.evon.zone;

import com.littleteam.evon.utils.*;
import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.extensions.SFSExtension;

/**
 *
 * @author PanCrucian
 */
public class EvonZoneExtension extends SFSExtension{

    @Override
    public void init() {
        addEventHandler(SFSEventType.USER_DISCONNECT, LeaveUser.class);
        Log.Info(this, "Evon Zone Extension Initalized");
    }
    
    /**
    * Юзер вышел из зоны
    * @param user - смартфокс юзер
    */
   public void OnUserLeave(User user) {
           //Players.RemovePlayer(user);
           //Log.Info(this, "Удалил игрока из глобального массива игроков: " + user.getName() + ", зона");
   }
}
