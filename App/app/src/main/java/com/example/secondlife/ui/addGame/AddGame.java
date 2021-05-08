package com.example.secondlife.ui.addGame;

import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;

import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;
import androidx.navigation.Navigation;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.example.secondlife.R;
import com.example.secondlife.databinding.AddGameFragmentBinding;
import com.squareup.picasso.Picasso;

public class AddGame extends Fragment {

    private AddGameViewModel mViewModel;
    private AddGameFragmentBinding binding;

    public static AddGame newInstance() {
        return new AddGame();
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        binding = AddGameFragmentBinding.inflate(inflater, container, false);

        Observer<Boolean> o = success -> {
            if (success) Navigation.findNavController(binding.getRoot()).navigate(R.id.action_navigation_addGame_to_navigation_myProducts);
        };
        mViewModel.getSuccess().observe(getActivity(), o);

        binding.txtUrl.setOnFocusChangeListener(txtUrlChanged());
        binding.btnAddGame.setOnClickListener(btnAddGameClick());

        return binding.getRoot();
    }

    @Override
    public void onActivityCreated(@Nullable Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);
        mViewModel = new ViewModelProvider(this).get(AddGameViewModel.class);
        // TODO: Use the ViewModel
    }

    private View.OnFocusChangeListener txtUrlChanged() {
        return (v, hasFocus) -> {
            String url = binding.txtUrl.getText().toString();
            if (url != null && url != "") Picasso.get().load(url).into(binding.imgGame);
            else Picasso.get().load(R.drawable.ic_baseline_image_search_24).into(binding.imgGame);
        };
    }

    private View.OnClickListener btnAddGameClick() {
        return v -> {
            String title = binding.txtTitle.getText().toString();
            String desc = binding.txtDescription.getText().toString();
            String url = binding.txtUrl.getText().toString();
            Float price = Float.parseFloat(binding.txtPrice.getText().toString());
            setEnabled(false);
            mViewModel.addGame(title, desc, url, price);
        };
    }

    private void setEnabled(boolean value) {
        binding.txtTitle.setEnabled(value);
        binding.txtDescription.setEnabled(value);
        binding.txtUrl.setEnabled(value);
        binding.txtPrice.setEnabled(value);
        binding.btnAddGame.setEnabled(value);
    }

}