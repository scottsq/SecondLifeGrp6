package com.example.secondlife;

import android.app.Application;

public class LocalData extends Application {
    private int userId = -1;
    private String token = null;

    public int getUserId(){
        return userId;
    }
    public String getToken() { return token; }

    public void setUserId(int id){
        userId = id;
    }
    public void setToken(String token) { this.token = token; }
}
