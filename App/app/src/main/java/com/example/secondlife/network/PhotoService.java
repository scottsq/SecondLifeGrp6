package com.example.secondlife.network;

import com.example.secondlife.model.Photo;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface PhotoService {
    @GET("photo/{id}")
    Call<Photo> getPhoto(@Path("id") Integer id);

    @POST("photo")
    Call<Photo> createPhoto(@Body Photo photo);

    @PATCH("photo/{id}")
    Call<Photo> updatePhoto(@Path("id") Integer id, @Body Photo photo);

    @DELETE("photo/{id}")
    Call<Photo> deletePhoto(@Path("id") Integer id);
}
