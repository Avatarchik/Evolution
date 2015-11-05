/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.utils;

import static java.lang.Math.toIntExact;

/**
 * Работа со временем
 * @author PanCrucian
 */
public class Time {

    /**
     * Штамп Юникс в секундах
     * @return 
     */
    public static int GetUnixStampSeconds() {
        return toIntExact(System.currentTimeMillis() / 1000L);
    }
    
    /**
     * Штамп Юникс в миллисекундах
     * @return 
     */
    public static long GetUnixStampMillisecond() {
        return System.currentTimeMillis();
    }
}
