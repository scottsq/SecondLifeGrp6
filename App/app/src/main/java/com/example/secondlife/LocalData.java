package com.example.secondlife;

import android.app.Application;

public class LocalData extends Application {
    private int userId;

    public int getUserId(){
        return userId;
    }

    public void setUserId(int id){
        userId = id;
    }
}
