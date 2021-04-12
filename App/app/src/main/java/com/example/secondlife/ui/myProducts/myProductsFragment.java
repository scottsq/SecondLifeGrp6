package com.example.secondlife.ui.myProducts;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.secondlife.LocalData;
import com.example.secondlife.databinding.FragmentMyProductsBinding;
import com.example.secondlife.model.ProductWithPhoto;
import com.example.secondlife.network.ProductService;
import com.example.secondlife.ui.home.ProductRecyclerViewAdapter;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;

public class myProductsFragment extends Fragment {

    private myProductsViewModel dashboardViewModel;
    private FragmentMyProductsBinding binding;
    private LocalData localData = LocalData.GetInstance();
    private Retrofit retrofit = localData.GetRetrofit();
    private ProductService apiService = retrofit.create(ProductService.class);
    private List<ProductWithPhoto> products = new ArrayList<>();
    private myProductsRecyclerViewAdapter adapter;

    @Override
    public View onCreateView (LayoutInflater inflater,
                              ViewGroup container,
                              Bundle savedInstanceState) {

        apiService.getUserProductWithPhoto(localData.getUserId()).enqueue(getUserProductWithPhoto());
        binding = FragmentMyProductsBinding.inflate(inflater, container, false);
        View view = binding.getRoot();
        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }

    private Callback<List<ProductWithPhoto>> getUserProductWithPhoto() {
        return new Callback<List<ProductWithPhoto>>() {
            @Override
            public void onResponse(Call<List<ProductWithPhoto>> call, Response<List<ProductWithPhoto>> response) {
                products = response.body();
                // Pour le recyclerViewProduct
                adapter = new myProductsRecyclerViewAdapter(getActivity(), products, getContext());
                RecyclerView recyclerview = binding.recyclerViewMyProducts;
                recyclerview.setLayoutManager(new LinearLayoutManager(getContext()));
                recyclerview.setAdapter(adapter);

                // Refresh la liste
                adapter.notifyDataSetChanged();

            }

            @Override
            public void onFailure(Call<List<ProductWithPhoto>> call, Throwable t) {

            }
        };
    }
}