<%@ Page Language="C#" MasterPageFile="~/temp.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="right_col" role="main">

        <!-- top tiles -->
        <div class="row tile_count">
            <div class="col-md-2 col-sm-4 col-xs-6 tile_stats_count">
                <span class="count_top"><i class="fa fa-user"></i>Total Users</span>
                <div class="count">2500</div>
                <span class="count_bottom"><i class="green">4% </i>From last Week</span>
            </div>
            <div class="col-md-2 col-sm-4 col-xs-6 tile_stats_count">
                <span class="count_top"><i class="fa fa-clock-o"></i>Average Time</span>
                <div class="count">123.50</div>
                <span class="count_bottom"><i class="green"><i class="fa fa-sort-asc"></i>3% </i>From last Week</span>
            </div>
            <div class="col-md-2 col-sm-4 col-xs-6 tile_stats_count">
                <span class="count_top"><i class="fa fa-user"></i>Total Males</span>
                <div class="count green">2,500</div>
                <span class="count_bottom"><i class="green"><i class="fa fa-sort-asc"></i>34% </i>From last Week</span>
            </div>
            <div class="col-md-2 col-sm-4 col-xs-6 tile_stats_count">
                <span class="count_top"><i class="fa fa-user"></i>Total Females</span>
                <div class="count">4,567</div>
                <span class="count_bottom"><i class="red"><i class="fa fa-sort-desc"></i>12% </i>From last Week</span>
            </div>
            <div class="col-md-2 col-sm-4 col-xs-6 tile_stats_count">
                <span class="count_top"><i class="fa fa-user"></i>Total Collections</span>
                <div class="count">2,315</div>
                <span class="count_bottom"><i class="green"><i class="fa fa-sort-asc"></i>34% </i>From last Week</span>
            </div>
            <div class="col-md-2 col-sm-4 col-xs-6 tile_stats_count">
                <span class="count_top"><i class="fa fa-user"></i>Total Connections</span>
                <div class="count">7,325</div>
                <span class="count_bottom"><i class="green"><i class="fa fa-sort-asc"></i>34% </i>From last Week</span>
            </div>
        </div>

        <div class="col-sm-6 col-lg-3">
            <a href="javascript:void(0)" class="widget">
                <div class="widget-content widget-content-mini text-right clearfix">
                    <div class="widget-icon pull-left themed-background">
                        <i class="gi gi-cardio text-light-op"></i>
                    </div>
                    <h2 class="widget-heading h3">
                        <strong>
                            <span data-toggle="counter" data-to="28350"></span>
                        </strong>
                    </h2>
                </div>
            </a>
        </div>
    </div>
</asp:Content>
