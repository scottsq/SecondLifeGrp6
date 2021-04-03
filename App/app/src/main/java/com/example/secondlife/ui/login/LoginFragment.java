package com.example.secondlife.ui.login;

import androidx.fragment.app.FragmentTransaction;
import androidx.lifecycle.ViewModelProvider;

import android.content.Intent;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import android.provider.ContactsContract;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.View;
import android.view.ViewGroup;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.databinding.FragmentHomeBinding;
import com.example.secondlife.databinding.FragmentLoginBinding;
import com.example.secondlife.model.LoginResponse;
import com.example.secondlife.model.Photo;
import com.example.secondlife.model.User;
import com.example.secondlife.network.OkHttpClass;
import com.example.secondlife.network.UserService;
import com.example.secondlife.ui.ProductDetails;
import com.example.secondlife.ui.home.HomeViewModel;
import com.example.secondlife.ui.profil.ProfilFragment;
import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import java.util.Map;

import okhttp3.OkHttpClient;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class LoginFragment extends Fragment {

    private LoginViewModel loginViewModel;
    private FragmentLoginBinding binding;
//    private OkHttpClient testClient = OkHttpClass.getUnsafeOkHttpClient();
//    testClient
//    OkHttpClient.Builder builder = new OkHttpClient.Builder();
//    builder.
//    OkHttpClient httpClient = builder.build();

    Gson gson = new GsonBuilder()
            .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
            .create();
    private final Retrofit retrofit = new Retrofit.Builder()
            .baseUrl("http://10.0.2.2:61169/api/")
           // .baseUrl("https://10.0.2.2:44359/api/")
            .client(OkHttpClass.getUnsafeOkHttpClient().newBuilder().followRedirects(false).followSslRedirects(false).build())
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

        // clique sur le boutton
        binding.button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                // Pour les info du User
                UserService apiService = retrofit.create(UserService.class);
                User u = new User();
                u.setLogin(binding.editTextName.getText().toString());
                u.setPassword(binding.editTextPassword.getText().toString());

                apiService.loginUser(u).enqueue(new Callback<LoginResponse>() {
                    @Override
                    public void onResponse(Call<LoginResponse> call, Response<LoginResponse> response) {
                        LoginResponse check = response.body();
                        //Log.v("testcheck",check.getToken());

                        if (check.getToken() != null)
                        {
                            // Save values
                            LocalData localData = (LocalData)(getActivity().getApplication());
                            localData.setUserId(check.getId());
                            localData.setToken(check.getToken());

                            // Change bottom menu
                            BottomNavigationView navigation = (BottomNavigationView) getActivity().findViewById(R.id.nav_view);
                            navigation.getMenu().clear();
                            navigation.inflateMenu(R.menu.bottom_nav_menu_2);

                            // Change fragment
                            Fragment f = getParentFragmentManager().findFragmentById(R.id.navigation_profil);
                            FragmentTransaction ft = getActivity().getSupportFragmentManager().beginTransaction();
                            ft.setReorderingAllowed(true);
                            ft.replace(R.id.nav_host_fragment, ProfilFragment.class, null);
                            ft.commit();
                        }
                        else
                        {
                            Log.v("test1","tamer");
                        }
                    }

                    @Override
                    public void onFailure(Call<LoginResponse> call, Throwable t) {
                        ((LocalData)(getActivity().getApplication())).setUserId(0);

                        BottomNavigationView navigation = (BottomNavigationView) getActivity().findViewById(R.id.nav_view);
                        navigation.getMenu().clear();
                        navigation.inflateMenu(R.menu.bottom_nav_menu_2);

                        Fragment f = getParentFragmentManager().findFragmentById(R.id.navigation_profil);
                        FragmentTransaction ft = getActivity().getSupportFragmentManager().beginTransaction();
                        ft.setReorderingAllowed(true);
                        ft.replace(R.id.nav_host_fragment, ProfilFragment.class, null);
                        ft.commit();
                        
                        // Log error here since request failed
                        Log.i("test","fail");
                        t.printStackTrace();

                    }
                });
            }
        });

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