package com.example.secondlife.network;


import com.example.secondlife.model.Tag;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface TagService {
    @GET("tag/{id}")
    Call<Tag> getTag(@Path("id") Integer id);

    @POST("tag")
    Call<Tag> createTag(@Body Tag tag);

    @PATCH("tag/{id}")
    Call<Tag> updateTag(@Path("id") Integer id, @Body Tag tag);

    @DELETE("tag")
    Call<Tag> deleteTag(@Path("id") Integer id);
}
