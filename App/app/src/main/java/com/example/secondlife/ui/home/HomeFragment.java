package com.example.secondlife.ui.home;

import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.databinding.FragmentHomeBinding;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.Product;
import com.example.secondlife.model.User;
import com.example.secondlife.network.OkHttpClass;
import com.example.secondlife.network.ProductService;
import com.example.secondlife.network.UserService;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.squareup.picasso.Picasso;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Random;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;


public class HomeFragment extends Fragment {

    private ProductRecyclerViewAdapter adapter;
    private List<Product> products = new ArrayList<>();
    private List<Photo> photos = new ArrayList<>();

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
        //Initialisation
        homeViewModel = new ViewModelProvider(this).get(HomeViewModel.class);
        binding = FragmentHomeBinding.inflate(inflater, container, false);



        // Pour les info du User
        LocalData localData = LocalData.GetInstance();
        UserService apiService = retrofit.create(UserService.class);
        apiService.getUser(localData.getToken(),localData.getUserId()).enqueue(new Callback<User>() {
            @Override
            public void onResponse(Call<User> call, Response<User> response) {
                User user = response.body();
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
                products = response.body();
                for (int i = 0; i < products.size(); i++) {
                    Log.v("Name: ", products.get(i).getName());
                }

                //Pour le recyclerViewProduct
                adapter = new ProductRecyclerViewAdapter(getActivity(), products, photos, getContext());
                Log.v("Product list: ", products.toString());
                RecyclerView recyclerview = binding.recyclerViewProduct;
                recyclerview.setLayoutManager(new LinearLayoutManager(getContext()));
                recyclerview.setAdapter(adapter);

                adapter.notifyDataSetChanged(); // this refresh the list, only call it in ui thread

            }

            @Override
            public void onFailure(Call<List<Product>> call, Throwable t) {
                // Log error here since request failed
                Log.i("test","fail product");
                t.printStackTrace();
                Random r = new Random();
                for (int i=0; i<15; i++) {
                    Product p = new Product();
                    p.setId(i); p.setName("Product " + i); p.setPrice(r.nextInt(50));
                    products.add(p);
                    Photo ph = new Photo();
                    ph.setId(i);
                    ph.setUrl("https://whatflower.net/imgmini/" + (i+1) + ".png");
                    photos.add(ph);
                }

                adapter = new ProductRecyclerViewAdapter(getActivity(), products, photos, getContext());
                try {
                    RecyclerView recyclerview = binding.recyclerViewProduct;
                    recyclerview.setLayoutManager(new LinearLayoutManager(getContext()));
                    recyclerview.setAdapter(adapter);
                    adapter.notifyDataSetChanged(); // this refresh the list, only call it in ui thread
                } catch(Exception e) {} // do nothing, we just changed view while it was loading
            }
        });

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