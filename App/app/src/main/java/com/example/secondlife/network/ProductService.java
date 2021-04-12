package com.example.secondlife.network;

import com.example.secondlife.model.Product;
import com.example.secondlife.model.ProductWithPhoto;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface ProductService {
    @GET("product/{id}")
    Call<Product> getProduct(@Path("id") int id);

    @GET("product/user/{id}")
    Call<List<Product>> getUserProducts(@Path("id") int id);

    @GET("product/user/{id}/like")
    Call<List<Product>> getRelatedProducts(@Header("Authorization") String authorization, @Path("id") int id);

    @GET("product/latest")
    Call<List<Product>> getAllProduct();

    @GET("product/search")
    Call<List<Product>> search(@Query("keys") String keys);

    @GET("product/withphoto")
    Call<List<ProductWithPhoto>> getAllProductWithPhoto();

    @POST("product")
    Call<Product> createProduct(@Header("Authorization") String authorization, @Body Product product);

    @PATCH("product/{id}")
    Call<Product> updateProduct(@Header("Authorization") String authorization, @Path("id") int id, @Body Product product);

    @DELETE("product/{id}")
    Call<Product> deleteProduct(@Header("Authorization") String authorization, @Path("id") int id);
}
