package com.example.secondlife.model;

public class LoginResponse {
    private String token;
    private int id;

    public String getToken (){
        return token;
    }

    public int getId (){
        return id;
    }

    public void setToken (String token){
        this.token = token;
    }

    public void setId (int id){
        this.id = id;
    }
}
