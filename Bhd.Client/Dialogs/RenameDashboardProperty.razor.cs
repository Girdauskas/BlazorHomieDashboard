﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Bhd.Client.Services;
using Bhd.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bhd.Client.Dialogs {
    public partial class RenameDashboardProperty {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Inject]
        ISnackbar Snackbar { get; set; }

        [Inject]
        private IRestService RestService { get; set; }

        [Parameter]
        public string DashboardId { get; set; }

        [Parameter]
        public DashboardNode DashboardNode { get; set; }

        [Parameter]
        public DashboardProperty DashboardProperty { get; set; }

        private string _newPropertyName = "";

        protected override void OnParametersSet() {
            _newPropertyName = DashboardProperty.AlternativeName;
            base.OnParametersSet();
        }

        private void Cancel() {
            MudDialog.Cancel();
        }

        private async Task RenameProperty() {
            var dashboardConfigsResponse = await RestService.GetAsync<List<DashboardConfig>>("api/dashboards/configuration");
            var dashboardConfigs = dashboardConfigsResponse.Body;

            var dashboard = dashboardConfigs?.FirstOrDefault(d => d.DashboardId == DashboardId);
            var node = dashboard?.Nodes.FirstOrDefault(n => n.NodeName == DashboardNode.Name);
            var property = node?.Properties.FirstOrDefault(p => p.PropertyName == DashboardProperty.AlternativeName && p.PropertyPath == DashboardProperty.ActualPropertyPath);

            if (property != null) {
                property.PropertyName = _newPropertyName;
                await RestService.PutAsync("api/dashboards/configuration", dashboardConfigs);
                Snackbar.Add($"Property renamed to \"{_newPropertyName}\"", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            } else {
                Snackbar.Add($"Property \"{DashboardProperty.AlternativeName}\" not found.", Severity.Error);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
    }
}
