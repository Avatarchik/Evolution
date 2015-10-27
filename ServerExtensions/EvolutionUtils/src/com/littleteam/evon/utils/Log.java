/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.littleteam.evon.utils;

import com.smartfoxserver.v2.extensions.ExtensionLogLevel;
import com.smartfoxserver.v2.extensions.SFSExtension;

/**
 *
 * @author PanCrucian
 */
public class Log {

    /**
     * @param extension - расширение
     * @param message - сообщение
     */
    public static void Info(SFSExtension extension, String message) {
        extension.trace(ExtensionLogLevel.INFO, message);
    }

    /**
     * @param extension - расширение
     * @param message - сообщение
     */
    public static void Warning(SFSExtension extension, String message) {
        extension.trace(ExtensionLogLevel.WARN, message);
    }

}
