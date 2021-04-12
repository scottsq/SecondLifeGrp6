package com.example.secondlife.ui.login;

import androidx.lifecycle.ViewModelProvider;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.secondlife.LocalData;
import com.example.secondlife.R;
import com.example.secondlife.databinding.FragmentLoginBinding;
import com.example.secondlife.model.LoginResponse;
import com.example.secondlife.model.User;
import com.example.secondlife.network.UserService;
import com.google.android.material.bottomnavigation.BottomNavigationView;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;

public class LoginFragment extends Fragment {

    private LoginViewModel loginViewModel;
    private FragmentLoginBinding binding;
    private final Retrofit retrofit = LocalData.GetInstance().GetRetrofit();

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
        binding.button.setOnClickListener(connectClick());

        View view = binding.getRoot();
        return view;
    }

    private View.OnClickListener connectClick() {
        return v -> {
            // Pour les info du User
            UserService apiService = retrofit.create(UserService.class);
            User u = new User();
            u.setLogin(binding.editTextName.getText().toString());
            u.setPassword(binding.editTextPassword.getText().toString());
            apiService.loginUser(u).enqueue(postLogin());
        };
    }

    private Callback<LoginResponse> postLogin() {
        return new Callback<LoginResponse>() {
            @Override
            public void onResponse(Call<LoginResponse> call, Response<LoginResponse> response) {
                LoginResponse check = response.body();

                if (response.isSuccessful())
                {
                    // Save values
                    LocalData localData = LocalData.GetInstance();
                    localData.setUserId(check.getId());
                    localData.setToken(check.getToken());

                    // Change bottom menu
                    BottomNavigationView navigation = (BottomNavigationView) getActivity().findViewById(R.id.nav_view);
                    navigation.getMenu().clear();
                    navigation.inflateMenu(R.menu.bottom_nav_menu_2);
                    navigation.setSelectedItemId(R.id.navigation_profil);
                }
                else
                {
                    getActivity().findViewById(R.id.textError).setVisibility(View.VISIBLE);
                }
            }

            @Override
            public void onFailure(Call<LoginResponse> call, Throwable t) {
                // Juste pour test vu que j'ai pas accès à l'api TwT
                // Faudra mettre un message d'erreur
                LocalData.GetInstance().setUserId(0);

                // Change bottom menu
                BottomNavigationView navigation = (BottomNavigationView) getActivity().findViewById(R.id.nav_view);
                navigation.getMenu().clear();
                navigation.inflateMenu(R.menu.bottom_nav_menu_2);
                navigation.setSelectedItemId(R.id.navigation_profil);

                // Log error here since request failed
                Log.i("test","fail");
                t.printStackTrace();
            }
        };
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        loginViewModel = new ViewModelProvider(this).get(LoginViewModel.class);
        // TODO: Use the ViewModel
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }

}