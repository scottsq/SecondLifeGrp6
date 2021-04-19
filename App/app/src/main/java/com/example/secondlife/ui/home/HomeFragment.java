package com.example.secondlife.ui.home;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.fragment.app.Fragment;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;

import com.example.secondlife.databinding.FragmentHomeBinding;
import com.example.secondlife.model.ProductWithPhoto;

import java.util.List;


public class HomeFragment extends Fragment {

    private ProductRecyclerViewAdapter adapter;
    private Observer<List<ProductWithPhoto>> oProducts;
    private HomeViewModel homeViewModel;
    private FragmentHomeBinding binding;
    private final boolean DECODE_STREAM = true;
    private LinearLayoutManagerCustom linearLayoutManager;

    @Override
    public View onCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        //Initialisation
        homeViewModel = new ViewModelProvider(this).get(HomeViewModel.class);
        binding = FragmentHomeBinding.inflate(inflater, container, false);
        linearLayoutManager = new LinearLayoutManagerCustom(getContext());

        //Product
        setLoadingScreenVisible(true);
        oProducts = productWithPhotos -> {
            try {
                adapter = new ProductRecyclerViewAdapter(getActivity(), productWithPhotos, getContext());
                binding.recyclerViewProduct.setLayoutManager(linearLayoutManager);
                binding.recyclerViewProduct.setAdapter(adapter);
                adapter.notifyDataSetChanged();
                setLoadingScreenVisible(false);
            } catch (Exception e) {};
        };
        homeViewModel.getProductsLiveData().observe(getActivity(), oProducts);


        binding.btnRefresh.setOnClickListener(btnRefreshClick());

        View view = binding.getRoot();
        return view;
    }

    private View.OnClickListener btnRefreshClick() {
        return new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                setLoadingScreenVisible(true);
                homeViewModel.getApiProducts();
            }
        };
    }

    private void setLoadingScreenVisible(boolean visible) {
        linearLayoutManager.setCanScrollVertically(!visible);
        binding.recyclerViewProduct.setAlpha(visible ? 0.5f : 1f);
        binding.recyclerViewProduct.setEnabled(!visible);
        binding.recyclerViewProduct.setVerticalScrollBarEnabled(!visible);
        binding.imgLoading.setVisibility(visible ? View.VISIBLE : View.GONE);
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }


}