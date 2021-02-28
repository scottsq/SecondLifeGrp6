package com.example.secondlife.network;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;

public interface UserService {

    @GET("user/{id}")
    Call<ResponseBody> getUser(@Path("id") Integer id);
}
