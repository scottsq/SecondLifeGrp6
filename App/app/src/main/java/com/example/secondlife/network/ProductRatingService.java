package com.example.secondlife.network;

import com.example.secondlife.model.ProductRating;
import com.example.secondlife.model.Rating;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface ProductRatingService {

    @POST("productrating")
    Call<ProductRating> createProductRating(@Header("Authorization") String authorization, @Body ProductRating productRating);

}
