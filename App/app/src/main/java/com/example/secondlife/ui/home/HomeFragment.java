package com.example.secondlife.ui.home;

import android.os.Bundle;
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

import retrofit2.Retrofit;

public class HomeFragment extends Fragment {

    private HomeViewModel homeViewModel;
    private FragmentHomeBinding binding;

    private final Retrofit retrofit = new Retrofit.Builder()
            .baseUrl("https://localhost/api/")
            .build();

    @Override
    public View onCreateView (LayoutInflater inflater,
                              ViewGroup container,
                              Bundle savedInstanceState) {

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