using System.Windows.Forms;
using CM3D2.MaidFiddler.Hook;
using CM3D2.MaidFiddler.Plugin.Utils;
using param;

namespace CM3D2.MaidFiddler.Plugin.Gui
{
    public partial class MaidFiddlerGUI
    {
        private bool updateFeature;
        private bool updatePropensity;

        private void InitMiscTab()
        {
            Debugger.Assert(
            () =>
            {
                Resources.GetFieldText(tabPage_misc);

                // Propensity
                Resources.GetFieldText(groupBox_propensity);
                for (Propensity e = Propensity.Null + 1; e < Propensity.Max; e++)
                {
                    checkedListBox_propensity.Items.Add(Resources.GetFieldText(EnumHelper.GetName(e)), false);
                }
                checkedListBox_propensity.Height = checkedListBox_propensity.ItemHeight
                                                   * checkedListBox_propensity.Items.Count;
                checkedListBox_propensity.ItemCheck += OnPropensityChecked;

                // Features
                Resources.GetFieldText(groupBox_feature);
                for (Feature e = Feature.Null + 1; e < Feature.Max; e++)
                {
                    checkedListBox_feature.Items.Add(Resources.GetFieldText(EnumHelper.GetName(e)), false);
                }
                checkedListBox_feature.Height = checkedListBox_feature.ItemHeight * checkedListBox_feature.Items.Count;
                checkedListBox_feature.ItemCheck += OnFeatureChecked;
            },
            "Failed to initialize propensity/feature tab");
        }

        private void OnFeatureChecked(object sender, ItemCheckEventArgs e)
        {
            if (clearingTables)
                return;

            MaidInfo maid = SelectedMaid;
            if (maid == null)
                return;

            if (!updateFeature)
            {
                maid.Maid.Param.SetFeature((Feature) (e.Index + 1), e.NewValue == CheckState.Checked);
                maid.UpdateMiscStatus(MaidChangeType.Feature, e.Index + 1);
            }
            updateFeature = false;
        }

        private void OnPropensityChecked(object sender, ItemCheckEventArgs e)
        {
            if (clearingTables)
                return;

            MaidInfo maid = SelectedMaid;
            if (maid == null)
                return;

            if (!updatePropensity)
            {
                maid.Maid.Param.SetPropensity((Propensity) (e.Index + 1), e.NewValue == CheckState.Checked);
                maid.UpdateMiscStatus(MaidChangeType.Propensity, e.Index + 1);
            }
            updatePropensity = false;
        }
    }
}