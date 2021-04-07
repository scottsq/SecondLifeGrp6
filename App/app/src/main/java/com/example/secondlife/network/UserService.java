package com.example.secondlife.network;

import com.example.secondlife.model.LoginResponse;
import com.example.secondlife.model.User;

import java.util.List;
import java.util.Map;

import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface UserService {

    @GET("user/{id}")
    Call<User> getUser(@Header("Authorization") String authorization, @Path("id") int id);

    @POST("user")
    Call<User> createUser(@Body User user);

    @POST("user/login")
    Call<LoginResponse> loginUser(@Body User user);

    @POST("user/reset")
    Call<User> resetPassword(@Body User user); // TODO: Changer type de retour

    @PATCH("user/{id}")
    Call<User> updateUser(@Path("id") int id, @Body User user);

    @DELETE("user/{id}")
    Call<User> deleteUser(@Path("id") int id);
}
