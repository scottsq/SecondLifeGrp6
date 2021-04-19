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

import androidx.lifecycle.Observer;

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
        Observer<Boolean> oIsConnected = value -> {
            if (!value) {
                getActivity().findViewById(R.id.textError).setVisibility(View.VISIBLE);
                binding.button.setEnabled(true);
                binding.editTextName.setEnabled(true);
                binding.editTextPassword.setEnabled(true);
                return;
            };
            BottomNavigationView navigation = (BottomNavigationView) getActivity().findViewById(R.id.nav_view);
            navigation.getMenu().clear();
            navigation.inflateMenu(R.menu.bottom_nav_menu_2);
            navigation.setSelectedItemId(R.id.navigation_profil);
        };
        loginViewModel.getIsConnected().observe(getActivity(), oIsConnected);

        binding = FragmentLoginBinding.inflate(inflater, container, false);

        // clique sur le boutton
        binding.button.setOnClickListener(connectClick());

        View view = binding.getRoot();
        return view;
    }

    private View.OnClickListener connectClick() {
        return v -> {
            binding.button.setEnabled(false);
            binding.editTextName.setEnabled(false);
            binding.editTextPassword.setEnabled(false);
            loginViewModel.Login(binding.editTextName.getText().toString(), binding.editTextPassword.getText().toString());
        };
    }

    @Override
    public void onDestroyView() {
        super.onDestroyView();
        binding = null;
    }

}