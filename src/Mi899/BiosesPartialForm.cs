﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Mi899.Data;
using System.Linq;
using System.Diagnostics;

namespace Mi899
{
    public partial class BiosesPartialForm : UserControl, II18nCompatible
    {
        private readonly Model _model;

        public BiosesPartialForm(Model model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            InitializeComponent();
            InitializeDataGridComponent();
        }

        public void ApplyI18n(I18n i18n)
        {
            this.ApplyI18nToChildren(i18n);
        }

        public IEnumerable<IComponent> SelectI18nCompatibleComponents()
        {
            List<IComponent> components = new List<IComponent>()
            {
                lblSearch,
                btnSearch
            };

            components.AddRange(grdBioses.Columns.OfType<IComponent>());

            return components;
        }

        private void InitializeDataGridComponent()
        {
            grdBioses.RowTemplate.Height = 80;

            grdBioses.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colName",
                HeaderText = nameof(BiosRowData.Name),
                DataPropertyName = nameof(BiosRowData.Name),
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle() 
                { 
                    WrapMode = DataGridViewTriState.True 
                }
            });

            grdBioses.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colProperties",
                HeaderText = nameof(BiosRowData.Properties),
                DataPropertyName = nameof(BiosRowData.PropertiesString),
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    WrapMode = DataGridViewTriState.True
                }
            });

            grdBioses.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colDescription",
                HeaderText = nameof(BiosRowData.Description),
                DataPropertyName = nameof(BiosRowData.Description),
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    WrapMode = DataGridViewTriState.True
                }
            });

            grdBioses.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colTags",
                HeaderText = nameof(BiosRowData.Tags),
                DataPropertyName = nameof(BiosRowData.TagsString),
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    WrapMode = DataGridViewTriState.True
                }
            });

            grdBioses.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colChipsets",
                HeaderText = nameof(BiosRowData.Chipsets),
                DataPropertyName = nameof(BiosRowData.ChipsetsStrings),
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    WrapMode = DataGridViewTriState.True
                }
            });

            grdBioses.Columns.Add(new DataGridViewLinkColumn()
            {
                Name = "colFileName",
                HeaderText = "File path",
                DataPropertyName = nameof(BiosRowData.FileName),
                ReadOnly = true,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                SortMode = DataGridViewColumnSortMode.Automatic,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    WrapMode = DataGridViewTriState.True
                }
            });

            PopulateDataSource();
            grdBioses.AutoGenerateColumns = false;
            grdBioses.DataSource = bsBioses;
            grdBioses.AutoResizeColumns();
        }

        private void PopulateDataSource()
        {
            string key = txtSearch.Text;
            IEnumerable<BiosRowData> source = _model.Bioses.Select(x => new BiosRowData(x));

            if (!string.IsNullOrWhiteSpace(key))
            { 
                source = source.Where
                (
                    x => x.Name.Contains(key, StringComparison.OrdinalIgnoreCase)
                        || x.TagsString.Contains(key, StringComparison.OrdinalIgnoreCase)
                        || x.Description.Contains(key, StringComparison.OrdinalIgnoreCase)
                        || x.PropertiesString.Contains(key, StringComparison.OrdinalIgnoreCase)
                );
            }

            bsBioses.DataSource = new GenericBindingList<BiosRowData>(source);
        }

        private void grdBioses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;

            if (grid.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex >= 0)
            {
                GenericBindingList<BiosRowData> list = (GenericBindingList<BiosRowData>)bsBioses.DataSource;

                BiosRowData bios = list[e.RowIndex];
                ProcessStartInfo psi = new ProcessStartInfo(bios.FileName)
                {
                    UseShellExecute = true
                };

                Process.Start(psi);
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                PopulateDataSource();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            PopulateDataSource();
        }
    }
}
