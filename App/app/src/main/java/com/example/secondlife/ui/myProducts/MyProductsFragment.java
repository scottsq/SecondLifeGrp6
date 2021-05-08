package com.example.secondlife.ui.myProducts;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.fragment.app.Fragment;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.databinding.FragmentMyProductsBinding;
import com.example.secondlife.model.ProductWithPhoto;
import com.example.secondlife.ui.home.LinearLayoutManagerCustom;
import com.example.secondlife.ui.home.ProductRecyclerViewAdapter;

import java.util.ArrayList;
import java.util.List;

import androidx.fragment.app.FragmentTransaction;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;
import androidx.navigation.Navigation;
import retrofit2.Retrofit;

public class MyProductsFragment extends Fragment {

    private MyProductsViewModel myProductsViewModel;
    private FragmentMyProductsBinding binding;
    private MyProductsRecyclerViewAdapter adapter;
    private LinearLayoutManagerCustom linearLayoutManager;

    @Override
    public View onCreateView (LayoutInflater inflater,
                              ViewGroup container,
                              Bundle savedInstanceState) {

        myProductsViewModel = new ViewModelProvider(this).get(MyProductsViewModel.class);
        binding = FragmentMyProductsBinding.inflate(inflater, container, false);
        linearLayoutManager = new LinearLayoutManagerCustom(getContext());

        binding.btnAddGame.setOnClickListener(btnAddGameClick());
        setLoadingScreenVisible(true);
        Observer<List<ProductWithPhoto>> products = productWithPhotos -> {
            try {
                adapter = new MyProductsRecyclerViewAdapter(getActivity(), productWithPhotos, getContext());
                binding.recyclerViewMyProducts.setLayoutManager(linearLayoutManager);
                binding.recyclerViewMyProducts.setAdapter(adapter);
                adapter.notifyDataSetChanged();
                setLoadingScreenVisible(false);
            } catch (Exception e) {};
        };

        View view = binding.getRoot();
        return view;
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }

    private void setLoadingScreenVisible(boolean visible) {
        linearLayoutManager.setCanScrollVertically(!visible);
        binding.recyclerViewMyProducts.setAlpha(visible ? 0.5f : 1f);
        binding.recyclerViewMyProducts.setEnabled(!visible);
        binding.recyclerViewMyProducts.setVerticalScrollBarEnabled(!visible);
        binding.imgLoading.setVisibility(visible ? View.VISIBLE : View.GONE);
    }

    private View.OnClickListener btnAddGameClick() {
        return v -> {
            Navigation.findNavController(v).navigate(R.id.action_navigation_myProducts_to_navigation_addGame);
        };
    }
}