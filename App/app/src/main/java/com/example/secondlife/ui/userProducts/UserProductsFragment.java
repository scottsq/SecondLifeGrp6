package com.example.secondlife.ui.userProducts;

import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.fragment.app.Fragment;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.secondlife.LocalData;
import com.example.secondlife.databinding.FragmentHomeBinding;
import com.example.secondlife.databinding.FragmentUserProductsBinding;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.Product;
import com.example.secondlife.model.User;
import com.example.secondlife.network.OkHttpClass;
import com.example.secondlife.network.ProductService;
import com.example.secondlife.network.UserService;
import com.example.secondlife.ui.home.HomeViewModel;
import com.example.secondlife.ui.home.ProductRecyclerViewAdapter;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class UserProductsFragment extends Fragment {
    private ProductRecyclerViewAdapter adapter;
    private List<Product> products = new ArrayList<>();
    private List<Photo> photos = new ArrayList<>();

    private UserProductsViewModel userProductsViewModel;
    private FragmentUserProductsBinding binding;


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
        userProductsViewModel = new ViewModelProvider(this).get(UserProductsViewModel.class);
        binding = FragmentUserProductsBinding.inflate(inflater, container, false);



        // Pour les info du User
        LocalData localData = (LocalData)(getActivity().getApplication());
        int id = localData.getUserId();

        if(id != -1){
            //Product
            ProductService apiServiceProduct = retrofit.create(ProductService.class);
            apiServiceProduct.getUserProducts(id).enqueue(new Callback<List<Product>>() {
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
                    // TODO: bouton pour aller sur login
                }
            });
        }else{
            // TODO
        }


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
