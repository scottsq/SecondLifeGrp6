package com.example.secondlife.network;

import com.example.secondlife.model.Proposal;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.PATCH;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface ProposalService {
    @GET("proposal/{id}")
    Call<Proposal> getProposal(@Path("id") Integer id);

    @POST("proposal")
    Call<Proposal> createProposal(@Body Proposal proposal);

    @PATCH("proposal/{id}")
    Call<Proposal> updateProposal(@Path("id") Integer id, @Body Proposal proposal);

    @DELETE("proposal")
    Call<Proposal> deleteProposal(@Path("id") Integer id);
}
