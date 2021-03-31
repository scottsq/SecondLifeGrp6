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
    @GET("rating/product/{id}")
    Call<Rating> getProductRating(@Path("id") int id);

    @GET("rating/user/{id}")
    Call<Rating> getUserRating(@Path("id") int id);

    @POST("rating")
    Call<Rating> createRating(@Body Rating rating);

    @PATCH("rating/{id}")
    Call<Rating> updateRating(@Path("id") int id, @Body Rating rating);

    @DELETE("rating")
    Call<Rating> deleteRating(@Path("id") int id);
}
