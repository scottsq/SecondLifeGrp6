package com.example.secondlife.ui.myProducts;

import com.example.secondlife.LocalData;
import com.example.secondlife.model.ProductWithPhoto;
import com.example.secondlife.network.ProductService;

import java.util.ArrayList;
import java.util.List;

import androidx.lifecycle.LiveData;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;

public class MyProductsViewModel extends ViewModel {

    private static MutableLiveData<List<ProductWithPhoto>> productsLiveData;
    private static List<ProductWithPhoto> mProducts = new ArrayList<>();
    private LocalData mLocalData = LocalData.GetInstance();
    private Retrofit mRetrofit = mLocalData.GetRetrofit();
    private ProductService mApiService = mRetrofit.create(ProductService.class);
    private static boolean isFetchingData = false;

    public MyProductsViewModel() {
        if (productsLiveData == null) productsLiveData = new MutableLiveData<>();
        if (mProducts.size() == 0 && !isFetchingData) getApiUserProducts();
    }

    public LiveData<List<ProductWithPhoto>> getProductsLiveData() {
        if (productsLiveData == null) productsLiveData = new MutableLiveData<>();
        return productsLiveData;
    }

    private void getApiUserProducts() {
        if (mProducts != null) mProducts.clear();
        isFetchingData = true;
        mApiService.getUserProductWithPhoto(mLocalData.getUserId()).enqueue(getUserProductWithPhoto());
    }


    private Callback<List<ProductWithPhoto>> getUserProductWithPhoto() {
        return new Callback<List<ProductWithPhoto>>() {
            @Override
            public void onResponse(Call<List<ProductWithPhoto>> call, Response<List<ProductWithPhoto>> response) {
                productsLiveData.setValue(response.body());
                isFetchingData = false;
            }

            @Override
            public void onFailure(Call<List<ProductWithPhoto>> call, Throwable t) {
                productsLiveData.setValue(new ArrayList<>());
                isFetchingData = false;
            }
        };
    }
}