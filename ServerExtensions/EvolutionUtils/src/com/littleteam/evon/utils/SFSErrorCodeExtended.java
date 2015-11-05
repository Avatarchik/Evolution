package com.littleteam.evon.utils;

import com.smartfoxserver.v2.exceptions.IErrorCode;

/**
 * Расширенный enum ошибок для смартфокса
 * @author PanCrucian
 */
public enum SFSErrorCodeExtended implements IErrorCode {    
    VERSION_NOT_VALID(1000),
    DATABASE_ERROR(1001),
    USERNAME_LENGHT(1002);
    private final short id;

    private SFSErrorCodeExtended(int id) {
        this.id = (short) id;
    }

    @Override
    public short getId() {
        return this.id;
    }
}

