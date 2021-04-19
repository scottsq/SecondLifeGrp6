package com.example.secondlife.ui.productDetails;

import android.view.View;

import com.example.secondlife.LocalData;
import com.example.secondlife.model.ProductRating;
import com.example.secondlife.network.ProductRatingService;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;

public class ProductDetailsViewModel extends ViewModel {

    private Retrofit mRetrofit = LocalData.GetInstance().GetRetrofit();
    private ProductRatingService mApiService = mRetrofit.create(ProductRatingService.class);
    private MutableLiveData<Float> mAverageLiveData;
    private MutableLiveData<ProductRating> mUserRating;

    public ProductDetailsViewModel() {
        if (mAverageLiveData == null) mAverageLiveData = new MutableLiveData<>();
        if (mUserRating == null) mUserRating = new MutableLiveData<>();
    }

    public MutableLiveData<Float> getAverageLiveData() {
        return mAverageLiveData;
    }

    public MutableLiveData<ProductRating> getProductRatingMutableLiveData() {
        return mUserRating;
    }

    public void getProductAverage(int productId) {
        mApiService.getAverage(productId).enqueue(getRatingStars());
    }
    private Callback<Float> getRatingStars(){
        return new Callback<Float>() {
            @Override
            public void onResponse(Call<Float> call, Response<Float> response) {
                mAverageLiveData.setValue(response.body());
            }

            @Override
            public void onFailure(Call<Float> call, Throwable t) {

            }
        };
    }

    public void getUserRating(int productId, int userId) {
        mApiService.getUserRatingForProduct(productId, userId).enqueue(getUserRatingApi());
    }

    public void createProductRating(ProductRating rating) {
        mApiService.createProductRating(LocalData.GetInstance().getToken(), rating).enqueue(new Callback<ProductRating>() {
            @Override
            public void onResponse(Call<ProductRating> call, Response<ProductRating> response) {
                getProductAverage(rating.getProduct().getId()); // refresh rating
            }

            @Override
            public void onFailure(Call<ProductRating> call, Throwable t) {
                // do nothing
            }
        });
    }

    private Callback<ProductRating> getUserRatingApi(){
        return new Callback<ProductRating>() {
            @Override
            public void onResponse(Call<ProductRating> call, Response<ProductRating> response) {
                if (!response.isSuccessful()){
                    mUserRating.setValue(null);
                } else mUserRating.setValue(response.body());
            }

            @Override
            public void onFailure(Call<ProductRating> call, Throwable t) {
                // Do nothing
            }
        };
    }

}