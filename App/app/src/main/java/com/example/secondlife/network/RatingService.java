package com.example.secondlife.network;


import com.example.secondlife.model.Rating;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface RatingService {
    @GET("rating/{id}")
    Call<Rating> getRating(@Path("id") Integer id);

    @POST("rating")
    Call<Rating> createRating(@Body Rating rating);

    @PATCH("rating/{id}")
    Call<Rating> updateRating(@Path("id") Integer id, @Body Rating rating);

    @DELETE("rating")
    Call<Rating> deleteRating(@Path("id") Integer id);
}
