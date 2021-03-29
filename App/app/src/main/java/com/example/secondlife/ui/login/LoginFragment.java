package com.example.secondlife.ui.login;

import androidx.lifecycle.ViewModelProvider;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.secondlife.R;
import com.example.secondlife.databinding.FragmentHomeBinding;
import com.example.secondlife.databinding.FragmentLoginBinding;
import com.example.secondlife.network.OkHttpClass;
import com.example.secondlife.ui.home.HomeViewModel;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class LoginFragment extends Fragment {

    private LoginViewModel loginViewModel;
    private FragmentLoginBinding binding;

    Gson gson = new GsonBuilder()
            .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
            .create();
    private final Retrofit retrofit = new Retrofit.Builder()
            .baseUrl("http://10.0.2.2:61169/api/")
            .client(OkHttpClass.getUnsafeOkHttpClient())
            .addConverterFactory(GsonConverterFactory.create(gson))
            .build();

    public static LoginFragment newInstance() {
        return new LoginFragment();
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {

        //Initialisation
        loginViewModel = new ViewModelProvider(this).get(LoginViewModel.class);
        binding = FragmentLoginBinding.inflate(inflater, container, false);



        View view = binding.getRoot();
        return view;
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        loginViewModel = new ViewModelProvider(this).get(LoginViewModel.class);
        // TODO: Use the ViewModel
    }

}