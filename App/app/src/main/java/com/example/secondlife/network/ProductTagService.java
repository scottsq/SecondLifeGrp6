package com.example.secondlife.network;


import com.example.secondlife.model.ProductTag;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface ProductTagService {
    @GET("producttag/{id}")
    Call<ProductTag> getProductTag(@Path("id") Integer id);

    @POST("producttag")
    Call<ProductTag> createProductTag(@Body ProductTag productTag);

    @PATCH("producttag/{id}")
    Call<ProductTag> updateProductTag(@Path("id") Integer id, @Body ProductTag productTag);

    @DELETE("producttag")
    Call<ProductTag> deleteProductTag(@Path("id") Integer id);
}
