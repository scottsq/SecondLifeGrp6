package com.example.secondlife;

import android.app.Application;

import com.google.gson.Gson;
import com.google.gson.JsonArray;
import com.google.gson.JsonNull;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

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

    public <T> JsonArray ObjectToPatch(T obj) {
        List<HashMap> list = new ArrayList<>();
        Field[] allFields = obj.getClass().getDeclaredFields();
        for (Field field : allFields) {
            HashMap hmap = new HashMap();
            hmap.put("op", "replace");
            hmap.put("path", "/" + field.getName());
            try {
                hmap.put("value", field.get(obj));
                if (field.get(obj) != null) list.add(hmap);
            }
            catch (Exception e) { new Gson().fromJson(new Gson().toJson(new ArrayList<HashMap>()), JsonArray.class); }
        }
        return new Gson().fromJson(new Gson().toJson(list), JsonArray.class);
    }
}
