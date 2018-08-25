<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ControlChart_Supplimentry.aspx.cs" Inherits="LatestVoterSearch.Chart.ControlChart_Supplimentry" MasterPageFile="~/Master/MainMaster.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-title">
        <div class="title_left">
            <h3>Control Chart</h3>
        </div>

        <div class="title_right">
            <div class="col-md-5 col-sm-5 col-xs-12 form-group pull-right top_search">
            </div>
        </div>
    </div>
    <div class="clearfix"></div>

    <div class="row">
        <div class="col-md-12 col-sm-12 col-xs-12">
            <div class="x_panel">
                <div class="x_content">
                    <br />
                    <div class="row">
                        <div class="form-group">
                            <label class="control-label col-md-2 col-sm-2 col-xs-12" for="last-name">
                                Select Assembly <span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <asp:DropDownList ID="ddlAssembly" runat="server" AutoPostBack="true" CssClass="form-control">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="clearfix"></div>
                    <div style="height: 10px"></div>
                    <div class="row">
                        <div class="form-group">
                            <label class="control-label col-md-2 col-sm-2 col-xs-12" for="first-name">
                                Select Local Body <span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <asp:DropDownList ID="ddlLocalBody" runat="server" AutoPostBack="true" CssClass="form-control">
                                    <asp:ListItem Value="0">Select</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="clearfix"></div>
                    <div style="height: 10px"></div>
                    <div class="row">
                        <div class="form-group">
                            <label class="control-label col-md-2 col-sm-2 col-xs-12" for="last-name">
                                Select Excel File <span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <asp:FileUpload ID="upldVoterList" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>

                    <div class="clearfix"></div>
                    <div style="height: 10px"></div>
                    <div class="row">
                        <div class="form-group">
                            <label class="control-label col-md-2 col-sm-2 col-xs-12" for="last-name">
                                <span class="required">*</span>
                            </label>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <asp:Button ID="btnSubmitSupplimentry" runat="server" Text="Submit" CssClass="btn btn-success" />
                                <asp:Button ID="btnCancle" runat="server" Text="Cancel" CssClass="btn btn-primary" />
                            </div>
                        </div>
                    </div>

                </div>

            </div>
        </div>
    </div>
</asp:Content>

