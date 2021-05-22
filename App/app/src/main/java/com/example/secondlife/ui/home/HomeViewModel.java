package com.example.secondlife.ui.home;

import android.util.Log;

import com.example.secondlife.LocalData;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.Product;
import com.example.secondlife.model.ProductWithPhoto;
import com.example.secondlife.model.User;
import com.example.secondlife.network.ProductService;
import com.google.gson.Gson;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;


public class HomeViewModel extends ViewModel {

    // static pour pas la recharger à chaque fois
    private static MutableLiveData<List<ProductWithPhoto>> productsLiveData;
    private static List<ProductWithPhoto> products = new ArrayList<>();
    private static boolean isFetchingData = false;
    private Retrofit mRetrofit = LocalData.GetInstance().GetRetrofit();

    public HomeViewModel() {
        if (productsLiveData == null) productsLiveData = new MutableLiveData<>();
        if (products != null && products.size() == 0 && !isFetchingData) getApiProducts(null);
    }

    public MutableLiveData<List<ProductWithPhoto>> getProductsLiveData() {
        if (productsLiveData == null) productsLiveData = new MutableLiveData<>();
        return productsLiveData;
    }

    public void getApiProducts(String keys) {
        if (isFetchingData) return;
        if (products != null) products.clear();
        isFetchingData = true;
        ProductService apiServiceProduct = mRetrofit.create(ProductService.class);
        if (keys.equals("")) keys = null;
        apiServiceProduct.findProductsWithPhoto(-1,-1,keys,null,"CreationDate",true,0,10).enqueue(getProductListResponse());
    }

    private Callback<List<ProductWithPhoto>> getProductListResponse() {
        return new Callback<List<ProductWithPhoto>>() {
            @Override
            public void onResponse(Call<List<ProductWithPhoto>> call, Response<List<ProductWithPhoto>> response) {
                Log.w("alalalal", new Gson().toJson(response.body()));
                productsLiveData.setValue(response.body());
                isFetchingData = false;
            }

            @Override
            public void onFailure(Call<List<ProductWithPhoto>> call, Throwable t) {
                // Log error here since request failed
                Log.i("alalalal","alalalal2");
                t.printStackTrace();
                productsLiveData.setValue(new ArrayList<>());
                isFetchingData = false;

                // Juste pour test, faudrait mettre un message d'erreur à la place
                /*Random r = new Random();
                for (int i=0; i<15; i++) {
                    User u = new User(); u.setName("Test");
                    ProductWithPhoto pwp = new ProductWithPhoto();
                    List<Photo> photoList = new ArrayList<>();
                    Product p = new Product();
                    p.setId(i); p.setName("Product " + i); p.setPrice(r.nextInt(50)); p.setOwner(u);
                    Photo ph = new Photo();
                    ph.setId(i);
                    ph.setUrl("https://whatflower.net/imgmini/" + (i+1) + ".png");
                    photoList.add(ph);
                    pwp.setPhotoList(photoList);
                    products.add(pwp);
                    pwp.setProduct(p);
                }
                productsLiveData.setValue(products);
                isFetchingData = false;*/
            }
        };
    }
}