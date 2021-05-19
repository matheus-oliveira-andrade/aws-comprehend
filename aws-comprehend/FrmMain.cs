using Amazon;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aws_comprehend
{
    public partial class FrmMain : Form
    {
        private const string AWS_PROFILE_NAME = "AWS Educate profile";
        private const string AWS_ARN_COMPREHEND_CLASSIFIER = "arn:aws:comprehend:us-east-1:717510493583:document-classifier-endpoint/aws-comprehend";
        private readonly RegionEndpoint _region = RegionEndpoint.USEast1;

        private AWSCredentials _awsCredentials;

        public FrmMain()
        {
            InitializeComponent();
        }

        // Get credentials in machine
        private void GetCredentials()
        {
            var chain = new CredentialProfileStoreChain();

            if (!chain.TryGetAWSCredentials(AWS_PROFILE_NAME, out _awsCredentials))
            {
                MessageBox.Show("Error on get credentials");
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            //await SendTextToAwsComprehendDectectSentiment();

            await SendTextToAwsComprehendCustomClassify();
        }

        private async Task SendTextToAwsComprehendCustomClassify()
        {
            GetCredentials();

            var client = new AmazonComprehendClient(_awsCredentials, _region);

            var request = new ClassifyDocumentRequest
            {
                EndpointArn = AWS_ARN_COMPREHEND_CLASSIFIER,
                Text = txtBox.Text,
            };

            ClassifyDocumentResponse response = await client.ClassifyDocumentAsync(request);

            string json = JsonConvert.SerializeObject(response, Formatting.Indented);

            richTextBox1.Clear();
            richTextBox1.AppendText(json);
        }

        private async Task SendTextToAwsComprehendDectectSentiment()
        {
            GetCredentials();

            var client = new AmazonComprehendClient(_awsCredentials, _region);

            var request = new DetectSentimentRequest
            {
                Text = txtBox.Text,
                LanguageCode = LanguageCode.Pt,
            };

            DetectSentimentResponse response = await client.DetectSentimentAsync(request);

            string json = JsonConvert.SerializeObject(response, Formatting.Indented);

            richTextBox1.Clear();
            richTextBox1.AppendText(json);
        }
    }
}
