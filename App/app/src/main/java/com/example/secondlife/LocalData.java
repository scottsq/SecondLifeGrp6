package com.example.secondlife;

import android.app.Application;

import com.example.secondlife.network.OkHttpClass;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.JsonArray;
import com.google.gson.JsonElement;
import com.google.gson.JsonNull;
import com.google.gson.JsonObject;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

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

    public Retrofit GetRetrofit() {
        Gson gson = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
                .create();
        return new Retrofit.Builder()
                .baseUrl("http://10.0.2.2:5000/api/")
                .client(OkHttpClass.getUnsafeOkHttpClient())
                .addConverterFactory(GsonConverterFactory.create(gson))
                .build();
    }

    public <T> JsonArray ObjectToPatch(T obj, List<String> fieldsToSet) {
        List<HashMap> list = new ArrayList<>();
        Field[] allFields = obj.getClass().getDeclaredFields();
        for (Field field : allFields) {
            if (fieldsToSet.size() > 0 && !fieldsToSet.contains(field.getName().toLowerCase())) continue;
            field.setAccessible(true);
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


//    public <T> JsonArray ObjectToPatch(T obj) {
//        List<HashMap> list = new ArrayList<>();
//        Field[] allFields = obj.getClass().getDeclaredFields();
//        for (Field field : allFields) {
//            if (field.getName().toLowerCase() == "id") continue;
//            HashMap hmap = new HashMap();
//            hmap.put("op", "replace");
//            hmap.put("path", "/" + field.getName());
//            try {
//                hmap.put("value", field.get(obj));
//                if (field.get(obj) != null) list.add(hmap);
//            }
//            catch (Exception e) { new Gson().fromJson(new Gson().toJson(new ArrayList<HashMap>()), JsonArray.class); }
//        }
//        return new Gson().fromJson(new Gson().toJson(list), JsonArray.class);
//    }

//    public <T> JSONArray ObjectToPatch(T obj) throws JSONException {
//        JSONArray array = new JSONArray();
//        Field[] allFields = obj.getClass().getDeclaredFields();
//        for (Field field : allFields) {
//            if (field.getName().toLowerCase() == "id") continue;
//            JSONObject jsonObj = new JSONObject();
//            jsonObj.put("op", "replace");
//            jsonObj.put("path", "/" + field.getName());
//            jsonObj.put("op", "replace");
//            try {
//                jsonObj.put("value", field.get(obj));
//                if (field.get(obj) != null) array.put(jsonObj);
//            }
//            catch (Exception e) { }
//        }
//        return array;
//    }
}
