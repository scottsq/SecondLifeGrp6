package com.example.secondlife.ui.myProducts;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.fragment.app.Fragment;

import com.example.secondlife.LocalData;
import com.example.secondlife.databinding.FragmentMyProductsBinding;
import com.example.secondlife.network.ProductService;

import retrofit2.Retrofit;

public class myProductsFragment extends Fragment {

    private myProductsViewModel dashboardViewModel;
    private FragmentMyProductsBinding binding;
    private LocalData localData = LocalData.GetInstance();
    private Retrofit retrofit = localData.GetRetrofit();
    private ProductService apiService = retrofit.create(ProductService.class);

    @Override
    public View onCreateView (LayoutInflater inflater,
                              ViewGroup container,
                              Bundle savedInstanceState) {

        //apiService.get
        binding = FragmentMyProductsBinding.inflate(inflater, container, false);
        View view = binding.getRoot();
        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }
}