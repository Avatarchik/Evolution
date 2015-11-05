/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.utils;

import com.smartfoxserver.v2.extensions.ExtensionLogLevel;
import com.smartfoxserver.v2.extensions.SFSExtension;

/**
 * Логирование
 * @author PanCrucian
 */
public class Log {

    /**
     * Информация
     * @param extension - расширение
     * @param message - сообщение
     */
    public static void Info(SFSExtension extension, Object message) {
        extension.trace(ExtensionLogLevel.INFO, message);
    }

    /**
     * Опасность
     * @param extension - расширение
     * @param message - сообщение
     */
    public static void Warning(SFSExtension extension, Object message) {
        extension.trace(ExtensionLogLevel.WARN, message);
    }

    /**
     * Ошибка
     * @param extension - расширение
     * @param message - сообщение
     */
    public static void Error(SFSExtension extension, Object message) {
        extension.trace(ExtensionLogLevel.ERROR, message);
    }
}
