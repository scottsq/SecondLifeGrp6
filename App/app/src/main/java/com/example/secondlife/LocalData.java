package com.example.secondlife;

import android.app.Application;

public class LocalData extends Application {
    private int userId = -1;
    private String token = null;
    private static LocalData instance;

    public int getUserId(){
        return userId;
    }
    public String getToken() { return "Bearer " + token; }
    public static LocalData GetInstance()
    {
        if (instance == null) instance = new LocalData();
        return instance;
    }

    public void setUserId(int id){
        userId = id;
    }
    public void setToken(String token) { this.token = token; }
}
