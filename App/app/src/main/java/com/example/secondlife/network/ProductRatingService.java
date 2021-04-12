package com.example.secondlife.network;

import com.example.secondlife.model.Product;
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
    @GET("productrating/product/{id}/average")
    Call<Float> getAverage(@Path("id") int id);

    @GET("productrating/{idProduct}/user/{idUser}")
    Call<ProductRating> getUserRatingForProduct(@Path("idProduct") int idProduct,@Path("idUser") int idUser);

    @POST("productrating")
    Call<ProductRating> createProductRating(@Header("Authorization") String authorization, @Body ProductRating productRating);
}
