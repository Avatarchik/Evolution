/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.serialization;

import com.smartfoxserver.v2.protocol.serialization.SerializableSFSType;

/**
 * Информация о фейсбук пользователе
 * @author PanCrucian
 */
public class FacebookUserInfoMy implements SerializableSFSType {

    public String Id = "";
    public String Email = "";
    public String FirstName = "";
    public String LastName = "";
    public int Gender = 0; //0 Unknow, 1 Male, 2 Female
    public String Locale = "";

    /**
     * Все в строку
     * @return 
     */
    public String getDump() {
        return "Dump:\n"
                + Id + "\n"
                + Email + "\n"
                + FirstName + "\n"
                + LastName + "\n"
                + String.valueOf(Gender) + "\n"
                + Locale;
    }
}
