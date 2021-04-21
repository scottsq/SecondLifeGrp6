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
    Call<Proposal> getProposal(@Path("id") int id);

    @GET("proposal/user/{id}/active")
    Call<Proposal> getActiveProposal(@Path("id") int id);

    @POST("proposal")
    Call<Proposal> createProposal(@Body Proposal proposal);

    @POST("proposal/accept")
    Call<Proposal> acceptProposal(@Body Proposal proposal);

    @POST("proposal/refuse")
    Call<Proposal> refuseProposal(@Body Proposal proposal);

    @POST("proposal/close")
    Call<Proposal> closeProposal(@Body Proposal proposal);

    @PATCH("proposal/{id}")
    Call<Proposal> updateProposal(@Path("id") int id, @Body Proposal proposal);

    @DELETE("proposal/{id}")
    Call<Proposal> deleteProposal(@Path("id") int id);
}
