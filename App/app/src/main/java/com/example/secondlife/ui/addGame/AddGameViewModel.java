package com.example.secondlife.ui.addGame;

import com.example.secondlife.LocalData;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.Product;
import com.example.secondlife.model.User;
import com.example.secondlife.network.PhotoService;
import com.example.secondlife.network.ProductService;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import androidx.navigation.Navigation;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;

public class AddGameViewModel extends ViewModel {

    private LocalData mLocalData = LocalData.GetInstance();
    private Retrofit mRetrofit = mLocalData.GetRetrofit();
    private ProductService mProductService = mRetrofit.create(ProductService.class);
    private PhotoService mPhotoService = mRetrofit.create(PhotoService.class);
    private MutableLiveData<Boolean> success = new MutableLiveData<>();

    public MutableLiveData<Boolean> getSuccess() { return success; }

    public void addGame(String title, String desc, String url, Float price) {
        Product p = new Product();
        User u = new User();
        u.setId(mLocalData.getUserId());
        p.setOwner(u);
        p.setName(title);
        p.setDescription(desc);
        p.setPrice(price);
        mProductService.createProduct(mLocalData.getToken(), p).enqueue(new Callback<Product>() {
            @Override
            public void onResponse(Call<Product> call, Response<Product> response) {
                if (!response.isSuccessful()) {
                    success.setValue(false);
                    return;
                }
                Product product = response.body();
                Photo photo = new Photo();
                photo.setUrl(url);
                photo.setProduct(product);
                mPhotoService.createPhoto(mLocalData.getToken(), photo).enqueue(new Callback<Photo>() {
                    @Override
                    public void onResponse(Call<Photo> call, Response<Photo> response) {
                        if (!response.isSuccessful()) {
                            success.setValue(false);

                        }
                        else
                        {
                            success.setValue(true);
                        }
                        return;
                    }

                    @Override
                    public void onFailure(Call<Photo> call, Throwable t) {
                        success.setValue(false);
                    }
                });
            }

            @Override
            public void onFailure(Call<Product> call, Throwable t) {
                success.setValue(false);
            }
        });
    }

    public void addPhoto(String url, int idProduct) {

    }
}