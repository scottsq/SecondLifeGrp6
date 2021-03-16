package com.example.secondlife.ui.home;

import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;

import com.example.secondlife.R;
import com.example.secondlife.databinding.FragmentHomeBinding;
import com.example.secondlife.model.Product;
import com.example.secondlife.model.User;
import com.example.secondlife.network.OkHttpClass;
import com.example.secondlife.network.ProductService;
import com.example.secondlife.network.UserService;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.io.IOException;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;


public class HomeFragment extends Fragment {

    private HomeViewModel homeViewModel;
    private FragmentHomeBinding binding;

    Gson gson = new GsonBuilder()
            .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
            .create();
    private final Retrofit retrofit = new Retrofit.Builder()
            .baseUrl("http://10.0.2.2:61169/api/")
            .client(OkHttpClass.getUnsafeOkHttpClient())
            .addConverterFactory(GsonConverterFactory.create(gson))
            .build();

    @Override
    public View onCreateView (LayoutInflater inflater,
                              ViewGroup container,
                              Bundle savedInstanceState) {

        UserService apiService = retrofit.create(UserService.class);

        int id = 1;
        apiService.getUser(id).enqueue(new Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {
                // int statusCode = response.code();
                User user = response.body();
                Log.v("test user" , "test");
//                try {
//                    Log.v("test user" , response.errorBody().string());
//                }
//                catch (IOException e){};
                Log.v("Name: ",user.getName());
                Log.v("Login :",user.getLogin());

                //getResources().getResourceEntryName(user.getId())
                //localhost:61169/api/user/1
            }

            @Override
            public void onFailure(Call<User> call, Throwable t) {
                // Log error here since request failed
                Log.i("test","fail");
                t.printStackTrace();

            }
        });

        //Product
        ProductService apiServiceProduct = retrofit.create(ProductService.class);
        apiServiceProduct.getAllProduct().enqueue(new Callback<List<Product>>() {
            @Override
            public void onResponse(Call<List<Product>> call, Response<List<Product>> response) {
                Log.v("test","ok product");
                List<Product> product = response.body();
                for (int i = 0; i < product.size(); i++) {
                    Log.v("Name: ", product.get(i).getName());
                }


            }

            @Override
            public void onFailure(Call<List<Product>> call, Throwable t) {
                // Log error here since request failed
                Log.i("test","fail product");
                t.printStackTrace();
            }
        });

        homeViewModel = new ViewModelProvider(this).get(HomeViewModel.class);

        binding = FragmentHomeBinding.inflate(inflater, container, false);
        View view = binding.getRoot();
        return view;

        // homeviewModel.lastestGameLiveData.observe()      Attend un changement de donnée de homeViewModel
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}