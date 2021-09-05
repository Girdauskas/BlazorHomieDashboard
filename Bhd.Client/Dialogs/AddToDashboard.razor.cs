﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bhd.Client.Services;
using Bhd.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhd.Client.Dialogs {
    public partial class AddToDashboard {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Inject]
        ISnackbar Snackbar { get; set; }

        [Inject]
        private IRestService RestService { get; set; }

        [Parameter]
        public string PropertyPath { get; set; }

        private List<DashboardConfig> _dashboards = new();

        private DashboardConfig _selectedDashboard;

        private NodeConfig _selectedDashboardNode;

        private string _propertyName;

        protected async override Task OnParametersSetAsync() {
            var property = await RestService.GetAsync<Property>(PropertyPath);
            _dashboards = await RestService.GetAsync<List<DashboardConfig>>("api/dashboards/configuration");
            _propertyName = property.Name;
            await base.OnParametersSetAsync();
        }

        private void Cancel() {
            MudDialog.Cancel();
        }

        private async Task Add() {
            if (_selectedDashboard == null || _selectedDashboardNode == null) {
                return;
            }

            _selectedDashboardNode.Properties.Add(new PropertyConfig { PropertyName = _propertyName, PropertyPath = PropertyPath });

            await RestService.PutAsync("api/dashboards/configuration", _dashboards);
            Snackbar.Add($"\"{_propertyName}\" added to \"{_selectedDashboard.DashboardName}\"", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
    }
}